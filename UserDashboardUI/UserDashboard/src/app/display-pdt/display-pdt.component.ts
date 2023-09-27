import { Component, Input } from '@angular/core';
import { EcommServiceService } from '../ecomm-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-display-pdt',
  templateUrl: './display-pdt.component.html',
  styleUrls: ['./display-pdt.component.css']
})
export class DisplayPdtComponent {
   img_link:string[]=[]; 

  constructor(private my_service:EcommServiceService, private router: Router){
    this.img_link = my_service.temperoary;
  }

  ngOnInit() {

      console.log(this.img_link);

  }

  onGetProductDummy(my_product:string){
    this.my_service.onGetProductDummy(my_product);
    console.log("Product is clicked");
    setTimeout( () => {
      this.router.navigate(['/product']);
      
    }, 800 );

  }


}
