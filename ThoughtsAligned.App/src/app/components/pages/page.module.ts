import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PageRoutingModule } from './page-routing.module';
import { ProjectsDetailComponent } from './projects-detail/project-details.component';
import { ProjectComponent } from './projects/project.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { HomeComponent } from './home/home.component';
import { NavBarComponent } from './navbar/nav-bar.component';
import { ContactComponent } from './contact/contact.component';
import { MasterPageComponent } from './master-page/master-page.component';


@NgModule({
  declarations: [
    ProjectsDetailComponent,
    ProjectComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    NavBarComponent,
    ContactComponent,
    MasterPageComponent
  ],
  imports: [
    CommonModule,
    PageRoutingModule
  ]
})
export class PageModule { }
