import { Component } from '@angular/core';
import { SharedService } from '../../core/services/shared.service';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent {
  constructor( private _sharedService: SharedService) { 
    this._sharedService.handleHeaderCss.emit(false);
  }
}
