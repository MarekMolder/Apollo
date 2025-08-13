import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IAddressEnriched extends IDomainId {
  streetName: string;
  buildingNr: number;
  city: string;
  province: string;
  country: string;
  name: string;
  unitNr?: number | null;
  postalCode: string;
  fullAddress: string;
}
