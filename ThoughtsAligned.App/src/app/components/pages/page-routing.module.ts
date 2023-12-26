import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ContactComponent } from './contact/contact.component';
import { ProjectComponent } from './projects/project.component';
import { ProjectsDetailComponent } from './projects-detail/project-details.component';
import { MasterPageComponent } from './master-page/master-page.component';

const routes: Routes = [
  {
    path: '', component: MasterPageComponent, 
    children: [
      { path: '', redirectTo:'home', pathMatch:'full' },
      { path: 'home', component: HomeComponent},
      { path: 'contact', component: ContactComponent},
      { path: 'project', component: ProjectComponent},
      { path: 'projects-detail', component: ProjectsDetailComponent}]
  }
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PageRoutingModule { }
