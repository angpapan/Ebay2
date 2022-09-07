import { Component, AfterViewInit, Input } from '@angular/core';
import * as L from 'leaflet';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements AfterViewInit {
  @Input() latitude: number;
  @Input() longitude: number;
  private map: any;

  private initMap(): void {
    this.map = L.map('map', {
      center: [ this.latitude, this.longitude ],
      zoom: 17
    });
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 19,
      minZoom: 3,
      attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(this.map);

    L.marker([ this.latitude, this.longitude ]).addTo(this.map);

  }

  constructor() { }

  ngAfterViewInit(): void {
    this.initMap();
    this.map.invalidateSize(false);
  }
  // for use in tab
  resize(){
    this.map.invalidateSize(false);
  }
}
