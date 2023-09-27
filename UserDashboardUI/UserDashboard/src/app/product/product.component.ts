import { Component } from '@angular/core';
import { EcommServiceService } from '../ecomm-service.service';
import { searchPost } from '../header/Search.model';
import { productPost } from '../header/Product.model';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent {
  individual_pdt:productPost;
  img_loc = "../../assets/images/";
  constructor(private my_service: EcommServiceService){
    this.individual_pdt = new productPost;
  }

  ngOnInit(){
    this.individual_pdt = this.my_service.individual_pdt_details;
    console.log("Product");
    console.log(this.individual_pdt);
  //  this.img_loc = this.img_loc+this.individual_pdt.imagesURL;

  this.img_loc = this.img_loc+this.individual_pdt.imagesURL;
  console.log(this.img_loc);
    // for(const items in this.individual_pdt){
    //   console.log(items);
    // }
  }
  
}
