import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EcommServiceService } from '../ecomm-service.service';

@Component({
  selector: 'app-vendor-product-upload',
  templateUrl: './vendor-product-upload.component.html',
  styleUrls: ['./vendor-product-upload.component.css']
})
export class VendorProductUploadComponent {
  productForm!: FormGroup;
  imageForm!: FormGroup;

  constructor(private my_service:EcommServiceService){

  }

  ngOnInit() {
    this.productForm = new FormGroup({
      'productName':new FormControl(null, Validators.required),

      'sellerId':new FormControl(null, Validators.required),


      'category':new FormControl(null, Validators.required),

      'tags':new FormControl(null, Validators.required),

      'descriptions':new FormControl(null, Validators.required),

      'quantity':new FormControl(null, Validators.required),

      'discount':new FormControl(null, Validators.required),

      'price':new FormControl(null, Validators.required),

    });

    this.imageForm = new FormGroup({
      'image':new FormControl(null, Validators.required),
    });
  };


  onSubmitProduct(){
    
  }
  onSubmitImage(){

  }
}
