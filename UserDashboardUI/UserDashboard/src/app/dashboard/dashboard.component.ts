import { Component } from '@angular/core';
import { EcommServiceService } from '../ecomm-service.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  constructor(public my_service:EcommServiceService){

  }
  

}
