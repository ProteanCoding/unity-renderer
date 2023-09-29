import {hashV1} from "@dcl/hashing";
import {Avatar} from "@dcl/schemas";
import {recoverAddressFromEthSignature} from "@dcl/crypto/dist/crypto";

export async function isImpostor(avatar: Avatar, profileHash: string, profileSignedHash: string, signer: string | undefined): Promise<boolean> {
  let checksum = await getProfileChecksum(avatar);
  return checksum != profileHash/* || getSigner(profileHash, profileSignedHash) != signer*/
}

export async function getProfileChecksum(avatar: Avatar): Promise<string> {
  const encoder = new TextEncoder()
  const payload = JSON.stringify([avatar.name, avatar.hasClaimedName, ...avatar.avatar.wearables])
  return await hashV1(encoder.encode(payload));
}

function getSigner(hash: string, signedHash: string): string {
  return recoverAddressFromEthSignature(signedHash, hash)
}
