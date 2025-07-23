import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IAddress extends IDomainId {
  streetName: string;
  buildingNr: number;
  city: string;
  province: string;
  country: string;
  name: string;
  unitNr: number;
}
