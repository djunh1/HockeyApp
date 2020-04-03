/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RinkEditComponent } from './rink-edit.component';

describe('RinkEditComponent', () => {
  let component: RinkEditComponent;
  let fixture: ComponentFixture<RinkEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RinkEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RinkEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
