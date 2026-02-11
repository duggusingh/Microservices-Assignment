import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductCreateComponent } from './components/product-create/product-create.component';
import { OrderCreateComponent } from './components/order-create/order-create.component';

@NgModule({
  declarations: [
    AppComponent, 
    ProductListComponent, 
    ProductCreateComponent, 
    OrderCreateComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: ProductListComponent },
      { path: 'add-product', component: ProductCreateComponent },
      { path: 'place-order', component: OrderCreateComponent }
    ])
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
export class AppModule { }
