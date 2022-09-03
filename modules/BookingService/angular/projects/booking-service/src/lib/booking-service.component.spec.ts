import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { BookingServiceComponent } from './booking-service.component';

describe('BookingServiceComponent', () => {
  let component: BookingServiceComponent;
  let fixture: ComponentFixture<BookingServiceComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ BookingServiceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookingServiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
