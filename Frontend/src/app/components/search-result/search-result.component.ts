import { Component } from '@angular/core';
import { searchPost } from '../header/Search.model';
import { EcommServiceService } from 'src/app/services/ecomm-service.service';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchResultComponent {

  img_lis:string[]=[];
  Product_details:searchPost[]=[];
  fetchedPdt:boolean=false;

  constructor(private my_service:EcommServiceService) {
    this.img_lis=my_service.temperoary;
  }

  ngOnInit(){
    this.fetchedPdt=true;
    for(let items of this.my_service.recieved_product ){
      console.log(items);
    }
    this.Product_details = this.my_service.recieved_product;
  }

}
