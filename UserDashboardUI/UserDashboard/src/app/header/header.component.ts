import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {  FormControl, FormGroup, Validators } from '@angular/forms';
import { map } from 'rxjs';
import { searchPost } from './Search.model';
import { EcommServiceService } from '../ecomm-service.service';
import { Router } from '@angular/router';

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

}
  

