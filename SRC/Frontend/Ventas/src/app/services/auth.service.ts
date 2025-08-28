// auth.service.ts
import { Injectable, inject } from '@angular/core';
import { Observable, map, tap } from 'rxjs';
import { GenericService } from './generic.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private api = inject(GenericService);

  login(nombre: string, pass: string, remember = true): Observable<boolean> {
    return this.api.post('Auth/login', { nombre, pass }).pipe(
      tap((res) => {

        console.log('[Auth/login] raw response:', res);
      }),
      map((res: any) => this.extractToken(res)),
      tap((token) => {
        if (token) {
          if (remember) localStorage.setItem('token', token);
          else sessionStorage.setItem('token', token);
        }
      }),
      map((token) => !!token) 
    );
  }

  getToken(): string | null {
    return localStorage.getItem('token') ?? sessionStorage.getItem('token');
  }

  logout() {
    localStorage.removeItem('token');
    sessionStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  private extractToken(res: any): string | null {
    if (!res) return null;

 
    if (typeof res === 'string') return res;

  
    if (typeof res.token === 'string') return res.token;
    if (typeof res.accessToken === 'string') return res.accessToken;
    if (typeof res.jwt === 'string') return res.jwt;

    if (typeof res.data?.token === 'string') return res.data.token;
    if (typeof res.result?.token === 'string') return res.result.token;
    if (typeof res.payload?.token === 'string') return res.payload.token;

 
    if (typeof res.data?.accessToken === 'string') return res.data.accessToken;
    if (typeof res.value?.token === 'string') return res.value.token;

    return null;
  }
}
