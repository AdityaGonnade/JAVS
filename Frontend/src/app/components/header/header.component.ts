import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { EcommServiceService } from 'src/app/services/ecomm-service.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {

  searchForm!: FormGroup;
 
  ngOnInit() {
    this.searchForm = new FormGroup({
      'searchQuery':new FormControl(null, Validators.required),
    });
  };

  constructor(private my_service:EcommServiceService, private router: Router){
    
  }

  onSubmit(){
    this.my_service.onSearch(this.searchForm);
    setTimeout( () => {
      this.router.navigate(['/search-result']);
      
    }, 800 );
  }

  openCart(){
    this.my_service.openCart();
    setTimeout( () => {
      this.router.navigate(['/cart']);
      
    }, 800 );
  }
}
