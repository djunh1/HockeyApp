/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RinkListComponent } from './rink-list.component';

describe('RinkListComponent', () => {
  let component: RinkListComponent;
  let fixture: ComponentFixture<RinkListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RinkListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RinkListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
