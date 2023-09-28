import { Injectable, OnInit } from '@angular/core';
import { map } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { searchPost } from './header/Search.model';
import { productPost } from './header/Product.model';
import { reviewGet } from './header/Review.module';
import { cartPost } from './header/Cart.module';

@Injectable({
  providedIn: 'root'
})
export class EcommServiceService {

  
  // searchForm!: FormGroup;
  recieved_product:any[]=[];
  product_rating=0;
  dummy_imgs=[ "img4.jpg","img5.jpg","img6.jpg","img9.jpg","img8.jpg","img7.jpg"];

  product_reviews:reviewGet[]=[];

  individual_pdt_details:any;

  constructor(private http:HttpClient) {
   
  }
  
  


  onSearch(searchForm:FormGroup){
    console.log(searchForm.value);
    this.recieved_product=[];
    this.http.post< {[key:string] :searchPost}>(
      
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
      console.log("Recieved");
      for(const items of response){
        console.log(items);
        
        this.recieved_product.push(items);

        
      }
    });

    

    
  }



  onGetProductDummy(seller_id:string, product_name:string){
    this.individual_pdt_details=null;
    this.product_reviews=[];
    this.product_rating=0;

    console.log("hi",seller_id);
    const pdt_url = "https://localhost:7221/ProductFetchingProduct/"+product_name+"/"+seller_id;
    
    this.http.post<any>(
      pdt_url,
      {},
   
    )
    .subscribe(response=>{
      console.log("Product Response---");
      console.log(response);
      console.log("inside pdt");
      
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

  
      const review_url = "https://localhost:7221/BuyerReview/"+product_name;  
    this.http.get<any>(
      review_url,
      
    )
    .subscribe(response=>{
      console.log("Review Response---");

      for(const items in response.review){
        let review_list = new reviewGet();
        
          review_list.BuyerId = response.review[items].buyerId,
          review_list.Description = response.review[items].description,
          review_list.Rating = response.review[items].rating,
          review_list.ImageURL = response.review[items].imageURL,
        
          console.log("Review List");
        console.log(review_list);
        this.product_reviews.push(review_list);

      }
      this.product_rating=response.avgRating;
      console.log(this.product_rating);
    });

 
    
    
  }


  updateCart(){
    let mycart:cartPost = new cartPost();
    mycart.sellerId=this.individual_pdt_details.seller_id;
    mycart.productName = this.individual_pdt_details.productName;
    mycart.buyerId = "12";
    mycart.quantity = 1;
    
    console.log("h1111");

    const pdt_url = "https://localhost:7221/BuyerCart"

    this.http.post<any>(
      pdt_url,
      {
        "sellerId":"1",
        "productName":"2",
        "buyerId":"3",
        "quantity":4
      },
   
    )
    .subscribe(response=>{
      console.log("Cart Response---");
      console.log(response);
    });

    
  }


// Vendors functions
  onSubmitProduct(){

  }

  onSubmitImage(){

  }

}
