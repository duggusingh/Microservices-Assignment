import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductCreateComponent } from './components/product-create/product-create.component';
import { OrderCreateComponent } from './components/order-create/order-create.component';

const routes: Routes = [
  { path: '', component: ProductListComponent },                // Home: Product Catalog
  { path: 'add-product', component: ProductCreateComponent },    // Admin: Add Product & Azure Upload
  { path: 'place-order', component: OrderCreateComponent },     // Transaction: Stock Validation
  { path: '**', redirectTo: '' }                                // Fallback
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
