import {call, put} from 'redux-saga/effects'
import {trackEvent} from 'shared/analytics/trackEvent'
import defaultLogger from 'lib/logger'
import {validateAvatar} from 'shared/profiles/schemaValidation'
import {ensureAvatarCompatibilityFormat} from 'lib/decentraland/profiles/transformations/profileToServerFormat'
import {REMOTE_AVATAR_IS_INVALID, RemoteProfileWithHash} from 'shared/profiles/types'
import {cachedRequest} from './cachedRequest'
import {profileWithHashSuccess} from "../../actions";

export function* fetchCatalystProfile(userId: string, version?: number) {
  try {
    const remoteProfile: RemoteProfileWithHash = yield call(cachedRequest, userId, version)

    let avatar = remoteProfile.profile.avatars[0]

    if (avatar) {
      avatar = ensureAvatarCompatibilityFormat(avatar)

      if (!validateAvatar(avatar)) {
        defaultLogger.warn(`Remote avatar for user is invalid.`, userId, avatar, validateAvatar.errors)
        trackEvent(REMOTE_AVATAR_IS_INVALID, {
          avatar
        })
        return null
      }

      // old lambdas profiles don't have claimed names if they don't have the "name" property
      avatar.hasClaimedName = !!avatar.name && avatar.hasClaimedName
      avatar.hasConnectedWeb3 = true

      yield put(profileWithHashSuccess(userId, remoteProfile))

      return avatar
    }
  } catch (error: any) {
    if (error.message !== 'Profile not found') {
      defaultLogger.warn(`Error requesting profile for auth check ${userId}, `, error)
    }
  }
  return null
}
