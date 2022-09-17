import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl: 'http://localhost:4200/',
    name: 'BookingService',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44369',
    redirectUri: baseUrl,
    clientId: 'BookingService_App',
    responseType: 'code',
    scope: 'offline_access BookingService',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44369',
      rootNamespace: 'EasyAbp.BookingService',
    },
    BookingService: {
      url: 'https://localhost:44371',
      rootNamespace: 'EasyAbp.BookingService',
    },
  },
} as Environment;
