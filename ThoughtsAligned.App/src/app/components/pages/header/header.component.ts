import { Component } from '@angular/core';
import { SharedService } from '../../core/services/shared.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  isUpdateHeaderCss:boolean=false;
  constructor( private _sharedService: SharedService) { 
    this._sharedService.handleHeaderCss.subscribe((value:boolean)=>{
      this.isUpdateHeaderCss=value;
    })
  }
}
