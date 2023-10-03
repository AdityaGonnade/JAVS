import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VendorProductUploadComponent } from './vendor-product-upload.component';

describe('VendorProductUploadComponent', () => {
  let component: VendorProductUploadComponent;
  let fixture: ComponentFixture<VendorProductUploadComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VendorProductUploadComponent]
    });
    fixture = TestBed.createComponent(VendorProductUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
