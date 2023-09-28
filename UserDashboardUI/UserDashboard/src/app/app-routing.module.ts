import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SearchResultComponent } from './search-result/search-result.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ProductComponent } from './product/product.component';
import { VendorDashboardComponent } from './vendor-dashboard/vendor-dashboard.component';
import { CartComponent } from './cart/cart.component';

const routes: Routes = [
  { path: 'search-result', component: SearchResultComponent },
  {path:'dashboard', component:DashboardComponent},
  {path:'product', component:ProductComponent},
  {path:'vendordashboard',component:VendorDashboardComponent},

  {path:'Cart',component:CartComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }