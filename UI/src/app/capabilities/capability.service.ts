import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService, ApiResult } from '../base.service';
import { Observable, map } from 'rxjs';
import { Apollo, gql } from 'apollo-angular';

import { Capability } from './capability';

@Injectable({ providedIn: 'root',
})
export class CapabilityService extends BaseService<Capability> {
    constructor(
          http: HttpClient,
          private apollo: Apollo) {
                  super(http);
}

get(id: number): Observable<Capability> {
    return this.apollo
      .query({
        query: gql`
          query GetCapabilityById($id: Int!) {
            cities(where: { id: { eq: $id } }) {
              nodes {
                id
                name
                lat
                lon
                countryId
              }
            }
          }
        `,
        variables: {
          id
        }
      })
      .pipe(map((result: any) =>
        result.data.cities.nodes[0]));
  }

  put(input: Capability): Observable<Capability> {
    return this.apollo
      .mutate({
        mutation: gql`
          mutation UpdateCapability($city: CityDTOInput!) {
            updateCity(cityDTO: $city) { 
              id
              name
              description
            }
          }
        `,
        variables: {
          city: input
        }
      }).pipe(map((result: any) =>
        result.data.updateCity));
  }

  post(item: Capability): Observable<Capability> {
    return this.apollo
      .mutate({
        mutation: gql`
          mutation AddCity($city: CityDTOInput!) {
            addCity(cityDTO: $city) { 
              id 
              name
              lat
              lon
              countryId
            }
          }
        `,
        variables: {
          city: item
        }
      }).pipe(map((result: any) =>
        result.data.addCity));
  }

    getData(pageIndex: number, pageSize: number, sortColumn: string, sortOrder: string, filterColumn: string | null, filterQuery: string | null): Observable<ApiResult<Capability>> {
    return this.apollo
      .query({
        query: gql`
          query GetCapabilitiesApiResult(
            $pageIndex: Int!,
            $pageSize: Int!,
            $sortColumn: String,
            $sortOrder: String,
            $filterColumn: String,
            $filterQuery: String) {
            citiesApiResult(
              pageIndex: $pageIndex
              pageSize: $pageSize
              sortColumn: $sortColumn
              sortOrder: $sortOrder
              filterColumn: $filterColumn
              filterQuery: $filterQuery
            ){ 
               data { 
                 id
                 name
                 description
               },
               pageIndex
               pageSize
               totalCount
               totalPages
               sortColumn
               sortOrder
               filterColumn
               filterQuery
             }
          }
        `,
        variables: {
          pageIndex,
          pageSize,
          sortColumn,
          sortOrder,
          filterColumn,
          filterQuery
        }
      })
      .pipe(map((result: any) =>
        result.data.capabilitiesApiResult));
  }

}
