import { ModuleWithProviders, NgModule } from '@angular/core';
import { BOOKING_SERVICE_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class BookingServiceConfigModule {
  static forRoot(): ModuleWithProviders<BookingServiceConfigModule> {
    return {
      ngModule: BookingServiceConfigModule,
      providers: [BOOKING_SERVICE_ROUTE_PROVIDERS],
    };
  }
}
