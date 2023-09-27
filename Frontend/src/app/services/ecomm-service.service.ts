import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { map } from 'rxjs';
import { productPost } from '../components/header/Product.model';
import { searchPost } from '../components/header/Search.model';

@Injectable({
  providedIn: 'root'
})
export class EcommServiceService {

  // searchForm!: FormGroup;
  recieved_product:any[]=[];
  temperoary=[ "img4.jpg","img5.jpg","img6.jpg","img9.jpg","img8.jpg","img7.jpg"];
  individual_pdt_details:any;

  constructor(private http:HttpClient) { }

  onSearch(searchForm: FormGroup){

    this.http.post< {[key:string] : searchPost}>(
      'https://localhost:7221/ProductFetchingProduct/SearchProduct', searchForm.value,
    ).pipe( map(responseData => {
        const postArray:searchPost[] =[];
        for (const key in responseData){
          if(responseData.hasOwnProperty(key)){
            postArray.push({...responseData[key],searchQuery:key});
          }
        }
        return postArray;
      })
    )
    .subscribe(response=>{
      for(const items of response){
        // console.log(items);
        this.recieved_product.push(items);
      }
    });
  }

  onGetProductDummy(seller_id:string, product_name:string){
    
    this.individual_pdt_details=null;
    // console.log("hi",seller_id);
    const pdt_url = "https://localhost:7221/ProductFetchingProduct/"+product_name+"/"+seller_id;
    
    this.http.post<any>(
      pdt_url,
      {},
    )
    .subscribe(response=>{
      // console.log("Product Response---");
      // console.log(response);
      // console.log("inside pdt");
      
      this.individual_pdt_details = new productPost();
      this.individual_pdt_details.id = response.id;
      this.individual_pdt_details.sellerId = response.sellerId;
      this.individual_pdt_details.productName = response.productName;
      this.individual_pdt_details.description = response.description;
      this.individual_pdt_details.imagesURL = response.imagesURL;
      this.individual_pdt_details.price = response.price;
      this.individual_pdt_details.category = response.category;
      console.log(this.individual_pdt_details);
    }); 
  }
}


// Search post from header
// product post form header