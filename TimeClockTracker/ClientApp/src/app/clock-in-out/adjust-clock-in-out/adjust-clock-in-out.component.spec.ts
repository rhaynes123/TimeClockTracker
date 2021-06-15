import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdjustClockInOutComponent } from './adjust-clock-in-out.component';

describe('AdjustClockInOutComponent', () => {
  let component: AdjustClockInOutComponent;
  let fixture: ComponentFixture<AdjustClockInOutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdjustClockInOutComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdjustClockInOutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
