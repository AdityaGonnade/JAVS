import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { authGuard } from './guards/auth.guard';
import { ResetComponent } from './components/reset/reset.component';
import { SignupVendorComponent } from './components/signup-vendor/signup-vendor.component';
import { CartComponent } from './components/cart/cart.component';
import { SearchResultComponent } from './components/search-result/search-result.component';
import { ProductComponent } from './components/product/product.component';

const routes: Routes = [
  {path:'login',component: LoginComponent},
  {path:'signup',component: SignupComponent},
  {path:'dashboard',component: DashboardComponent,canActivate:[authGuard]},
  {path:'reset',component:ResetComponent},
  {path:'signupvendor',component: SignupVendorComponent},
  {path:'cart',component: CartComponent},
  { path: 'search-result', component: SearchResultComponent },
  {path:'product', component:ProductComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
