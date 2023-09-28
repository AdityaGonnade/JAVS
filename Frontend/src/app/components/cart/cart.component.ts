import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent {
    numberOfItems : number = 0;
    items: any;

    constructor(
      private auth: AuthService
    ) {
        
    }

}
