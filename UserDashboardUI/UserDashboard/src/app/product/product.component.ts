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
  constructor(private my_service: EcommServiceService){
    this.individual_pdt = new productPost;
  }

  ngOnInit(){
    this.individual_pdt = this.my_service.individual_pdt_details;
    console.log("Product");
    console.log(this.individual_pdt);
    // for(const items in this.individual_pdt){
    //   console.log(items);
    // }
  }
  
}
