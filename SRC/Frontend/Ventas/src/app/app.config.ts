// src/app/app.config.ts
import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';

import { provideHttpClient, withFetch } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

import { providePrimeNG } from 'primeng/config';
import Aura from '@primeng/themes/aura';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),

    // Router (si usás rutas)
    provideRouter(routes),

    // HTTP (una sola vez, con fetch opcional)
    provideHttpClient(withFetch()),

    // Animaciones + Tema PrimeNG v18
    provideAnimationsAsync(),
    providePrimeNG({
      theme: { preset: Aura }
    }),
  ]
};
