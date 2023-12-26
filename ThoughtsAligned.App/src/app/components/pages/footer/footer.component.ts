import { Component } from '@angular/core';
import { SharedService } from '../../core/services/shared.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent {
  isUpdateHeaderCss:boolean=false;
  constructor( private _sharedService: SharedService) { 
    this._sharedService.handleHeaderCss.subscribe((value:boolean)=>{
      this.isUpdateHeaderCss=value;
    })
  }
}
