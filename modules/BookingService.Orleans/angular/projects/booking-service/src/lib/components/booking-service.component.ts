import { Component, OnInit } from '@angular/core';
import { BookingServiceService } from '../services/booking-service.service';

@Component({
  selector: 'lib-booking-service',
  template: ` <p>booking-service works!</p> `,
  styles: [],
})
export class BookingServiceComponent implements OnInit {
  constructor(private service: BookingServiceService) {}

  ngOnInit(): void {
    this.service.sample().subscribe(console.log);
  }
}
