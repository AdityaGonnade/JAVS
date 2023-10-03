import { Component } from '@angular/core';
import { searchPost } from './header/Search.model';
import { EcommServiceService } from './ecomm-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})


export class AppComponent {
  title = 'UserDashboard';

  search_response:searchPost[] = [];  
  constructor(public my_service:EcommServiceService){

  }


  
}
