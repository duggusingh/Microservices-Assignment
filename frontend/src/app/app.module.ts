import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductCreateComponent } from './components/product-create/product-create.component';
import { OrderCreateComponent } from './components/order-create/order-create.component';

// Defining basic routes for navigation
const routes: Routes = [
  { path: '', component: ProductListComponent },
  { path: 'add-product', component: ProductCreateComponent },
  { path: 'place-order', component: OrderCreateComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [
    AppComponent,
    ProductListComponent,
    ProductCreateComponent,
    OrderCreateComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule, // Required for API communication with .NET services
    FormsModule,      // Required for template-driven forms
    ReactiveFormsModule, // Required for reactive forms (image uploads)
    RouterModule.forRoot(routes) // Sets up the routing for the SPA
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
