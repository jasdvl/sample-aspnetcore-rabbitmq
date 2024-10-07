import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FrontDeskComponent } from './features/front-desk/front-desk.component';

const routes: Routes = [
    { path: '', redirectTo: '/front-desk', pathMatch: 'full' },
    { path: 'front-desk', component: FrontDeskComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
