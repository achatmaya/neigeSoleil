import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalExempleComponent } from './modal-exemple.component';

describe('ModalExempleComponent', () => {
  let component: ModalExempleComponent;
  let fixture: ComponentFixture<ModalExempleComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ModalExempleComponent]
    });
    fixture = TestBed.createComponent(ModalExempleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
