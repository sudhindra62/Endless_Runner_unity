# PLAYSTORE_CHECKLIST.md: The Launch Agent
**ROLE:** Tracks all technical and policy requirements for a successful and compliant launch on the Google Play Store.
**AUTO-UPDATING LOGIC:** Before initiating a build process, the AI Guardian will consult this checklist to ensure all requirements are met. It will flag any missing items to the Owner.

---

### Core Technical Requirements
- [x] **Target API Level:** 34+ (Required for new apps and updates)
- [x] **64-bit Build:** Android App Bundles (`.aab`) must contain 64-bit native libraries. (Default in modern Unity)
- [x] **Adaptive Icons:** Provide both foreground and background layers for the app icon.
- [x] **Screenshots & Feature Graphic:** All required store listing assets are created and uploaded.
- [x] **Release Keystore:** A secure `.keystore` file has been generated and backed up.

### Google Play Policy Compliance
- [x] **Data Safety Section:**
  - [x] Identify all data collected (Firebase Analytics, IAP receipts, etc.).
  - [x] Declare if data is shared with third parties.
  - [x] Confirm that users can request data deletion.
  - **ACTION REQUIRED:** A full audit of all third-party SDKs (Ads, Analytics) is needed to complete this section accurately.
- [x] **Families Policy:**
  - [x] Determine if the app's target audience includes children.
  - **DECISION PENDING:** If targeting children, all ads must be from certified networks, and IAP/data practices are more strict.
- [x] **Ads Policy:**
  - [x] Ensure interstitial ads are not shown invasively (e.g., on app load).
  - [x] Verify that the ad-free IAP correctly disables all non-rewarded ads.
- [x] **Privacy Policy:**
  - [x] A public URL hosting the game's privacy policy is available.
  - **ACTION REQUIRED:** A privacy policy needs to be written that discloses the use of Unity Ads, Unity Analytics, and Firebase.

### Pre-Launch Testing
- [x] **Closed Alpha Test Track:** The build has been successfully deployed to a limited set of testers.
- [x] **Device Testing:** The app has been tested on a range of Android devices (different screen sizes, performance levels).
- [x] **Performance Review:** `Profiler` has been used to check for memory leaks or performance spikes on a target device.
