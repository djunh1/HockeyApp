/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RinkCardComponent } from './rink-card.component';

describe('RinkCardComponent', () => {
  let component: RinkCardComponent;
  let fixture: ComponentFixture<RinkCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RinkCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RinkCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
