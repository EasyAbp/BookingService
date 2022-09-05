import { NgModule, NgModuleFactory, ModuleWithProviders } from '@angular/core';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { BookingServiceComponent } from './components/booking-service.component';
import { BookingServiceRoutingModule } from './booking-service-routing.module';

@NgModule({
  declarations: [BookingServiceComponent],
  imports: [CoreModule, ThemeSharedModule, BookingServiceRoutingModule],
  exports: [BookingServiceComponent],
})
export class BookingServiceModule {
  static forChild(): ModuleWithProviders<BookingServiceModule> {
    return {
      ngModule: BookingServiceModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<BookingServiceModule> {
    return new LazyModuleFactory(BookingServiceModule.forChild());
  }
}
