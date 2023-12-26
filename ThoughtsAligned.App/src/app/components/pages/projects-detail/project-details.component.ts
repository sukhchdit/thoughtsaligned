import { Component } from '@angular/core';
import { SharedService } from '../../core/services/shared.service';

@Component({
  selector: 'app-project-details',
  templateUrl: './project-details.component.html',
  styleUrls: ['./project-details.component.css']
})
export class ProjectsDetailComponent {
  constructor( private _sharedService: SharedService) { 
    this._sharedService.handleHeaderCss.emit(false);
  }
}
