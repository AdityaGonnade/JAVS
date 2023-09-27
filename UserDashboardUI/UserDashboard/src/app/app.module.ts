import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { AdvertisementComponent } from './advertisement/advertisement.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SlickCarouselModule } from 'ngx-slick-carousel';
import { DisplayPdtComponent } from './display-pdt/display-pdt.component';
import { HttpClientModule } from '@angular/common/http';
import { SearchResultComponent } from './search-result/search-result.component';
import { EcommServiceService } from './ecomm-service.service';
import { AppRoutingModule } from './app-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FooterComponent } from './footer/footer.component';
import { ProductComponent } from './product/product.component';



@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    AdvertisementComponent,
    DisplayPdtComponent,
    SearchResultComponent,
    DashboardComponent,
    FooterComponent,
    ProductComponent,
    
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    SlickCarouselModule,
    ReactiveFormsModule,
    FormsModule,
    AppRoutingModule,
    
  ],
  providers: [EcommServiceService],
  bootstrap: [AppComponent]
})
export class AppModule { }
