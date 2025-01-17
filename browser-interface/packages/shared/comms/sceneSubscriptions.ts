import { PeerInformation } from './interface/types'

export interface ICommunicationsController {
  cid: string
  receiveCommsMessage(message: Uint8Array, sender: PeerInformation): void
}

export const scenesSubscribedToCommsEvents = new Set<ICommunicationsController>()

export function subscribeParcelSceneToCommsMessages(controller: ICommunicationsController) {
  scenesSubscribedToCommsEvents.add(controller)
}

export function unsubscribeParcelSceneToCommsMessages(controller: ICommunicationsController) {
  scenesSubscribedToCommsEvents.delete(controller)
}
