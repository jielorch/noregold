import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { FileUploadRepository } from '../../../core/repository/file-upload.repository';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormBuilder, FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-upload-excel',
  imports: [
    FormsModule
    ,ReactiveFormsModule
    ,CommonModule
  ],
  templateUrl: './upload-excel.html',
  styleUrl: './upload-excel.scss',
})
export class UploadExcel implements OnInit{

  fileUploadRepository = inject(FileUploadRepository);
  destroyRef = inject(DestroyRef);
  fb = inject(FormBuilder);

  fileForm = this.fb.group({
    file: new FormControl<File | null>(null,{ validators: [Validators.required] })
  });

  ngOnInit(): void {
      
  }

  loadData(){
    this.fileUploadRepository.get().pipe(
      takeUntilDestroyed(this.destroyRef)
    ).subscribe({
      next:(d) => {
        console.log(d);
      },
      error:(e) => {
        console.log(`error: ${e}`);
      }
    })
  }

  onFileSelected(event: Event){
    const input = event.target as HTMLInputElement;
    if(input.files && input.files.length > 0){
      const file = input.files[0];
      this.fileForm.patchValue({ file });
      this.fileForm.get('file')?.updateValueAndValidity();
    }
  }

  upload(){
     if(this.fileForm.valid){
        let formData = new FormData();
        const file = this.fileForm.get('file')?.value ?? '';
        formData.append('file', file);

        this.fileUploadRepository.uploadFile(formData).pipe(
          finalize(() => {
            console.log('done');
          })
        ).subscribe({
          next: (res) => {

          },
          error:(err) => {
            console.log('Error:', err);
          }
        })
     }else{
       alert('Upload a file');
     }
  }

}
