import { Injectable } from '@angular/core';

import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable, tap, throwError } from 'rxjs';

import { IApiBaseActions, ParamsType } from '../interfaces/Iapi-base-actions';
import { IResponse } from '../interfaces/Iresponse';

@Injectable({
  providedIn: 'root'
})
export class ApiHandlerService implements IApiBaseActions {

  constructor(public httpClient:HttpClient) { }
  
  private getHttpOptions(params?: ParamsType) {
    return {
      params: this.createParams(params),
      withCredentials: false  
    };
  }
  Get(url: string, params?: ParamsType): Observable<IResponse<any>> {
    return this.httpClient
      .get<IResponse<any>>(url, this.getHttpOptions(params)) 
      .pipe(tap((x) => this.handleResponse(x)));
  }

  GetAll(url: string, params?: ParamsType): Observable<IResponse<any>> {
     console.log('[ApiHandler] GET', url, params);
    return this.httpClient
      .get<IResponse<any[]>>(url, this.getHttpOptions(params)) 
      .pipe(
        tap((x) => this.handleResponse(x))
      );
  }

  Post(url: string, data: any, params?: ParamsType): Observable<IResponse<any>> {
    return this.httpClient
      .post<IResponse<any>>(url, data, this.getHttpOptions(params)) 
      .pipe(
        // catchError((err:HttpErrorResponse)=>{this.handleErrorResponse(err)})
      );
  }

  Delete(url: string, data?: any, params?: ParamsType): Observable<IResponse<any>> {
    
    return this.httpClient
      .delete<IResponse<any>>(url, this.getHttpOptions(params)) 
      .pipe(tap((x) => this.handleResponse(x)));
  }

  Put(url: string, data: any, params?: ParamsType): Observable<IResponse<any>> {
    return this.httpClient
      .put<IResponse<any>>(url, data, this.getHttpOptions(params)) 
      .pipe(tap((x) => { this.handleResponse(x); }));
  }

  handleResponse(response: any) {

    console.log(response);
  }

  handleErrorResponse(error: HttpErrorResponse): Observable<any> {
    return throwError(() => new Error(error.message));
  }

  createParams(params?: ParamsType) {
    let httpParams = new HttpParams();
    if (params) {
      Object.entries(params).forEach(([key, value]) => {
 
        httpParams = httpParams.append(key, String(value));
      });
    }
    return httpParams;
  }
}