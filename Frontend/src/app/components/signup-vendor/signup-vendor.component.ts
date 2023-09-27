import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import ValidateForm from 'src/app/helpers/validateform';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signup-vendor',
  templateUrl: './signup-vendor.component.html',
  styleUrls: ['./signup-vendor.component.css']
})
export class SignupVendorComponent {
  
  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  signUpForm!: FormGroup;

  constructor(
    private fb: FormBuilder, 
    private auth: AuthService, 
    private router: Router,
    private toast: NgToastService
  ) {}
    
  ngOnInit(): void{
      this.signUpForm = this.fb.group({
         firstName : ['',Validators.required],
         lastName: ['',Validators.required],
         email : ['',Validators.required],
         userName : ['',Validators.required],
         password : ['',Validators.required],
         mobileNumber : ['',Validators.required],
         pan : ['',Validators.required],
         gst : ['',Validators.required],
         accountNumber : ['',Validators.required]
      }) 
  }

  hideShowPassword(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type = "text" : this.type = "password";
  }

  onSignup(){
    if(this.signUpForm.valid){

      //push data into database
      // console.log(this.signUpForm.value);
      this.auth.signUpVendor(this.signUpForm.value)
      .subscribe({
        next: (res)=>{
          // alert(res.message);
          this.toast.success({detail:"SUCCESS", summary:res.message, duration: 5000});
          this.signUpForm.reset();
          this.router.navigate(['login']);
        },
        error:(err)=>{
          // console.log('Hello world' + err);
          alert(err.message);
          // this.toast.error({detail:"ERROR", summary:"Something wrong", duration: 5000});
        }
      })
    }
    else{

      ValidateForm.validateAllFormFields(this.signUpForm);
        //logic for throwing errors

    }
  }
}