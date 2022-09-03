import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class BookingServiceService {
  apiName = 'BookingService';

  constructor(private restService: RestService) {}

  sample() {
    return this.restService.request<void, any>(
      { method: 'GET', url: '/api/BookingService/sample' },
      { apiName: this.apiName }
    );
  }
}
