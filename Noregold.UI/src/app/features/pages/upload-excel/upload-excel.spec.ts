import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadExcel } from './upload-excel';

describe('UploadExcel', () => {
  let component: UploadExcel;
  let fixture: ComponentFixture<UploadExcel>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UploadExcel]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UploadExcel);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
