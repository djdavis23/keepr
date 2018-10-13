import Vue from 'vue'
import Vuex from 'vuex'
import Axios from 'axios'
import router from './router'

Vue.use(Vuex)

let auth = Axios.create({
  baseURL: "//localhost:5000/Account/",
  timeout: 3000,
  withCredentials: true
})

let api = Axios.create({
  baseURL: "//localhost:5000/api/",
  timeout: 3000,
  withCredentials: true,
})

export default new Vuex.Store({
  state: {
    user: {},
    keeps: [],
    vaults: [],
    vaultKeeps: {},
    activeVaultId: "",
    activeVaultKeeps: []
  },
  mutations: {

    //USER MUTATIONS
    setUser(state, user) {
      state.user = user
    },
    logout(state) {
      state.user = {}
      state.keeps = [];
      state.vaults = [];
      state.vaultKeeps = {};
      router.push('login')
    },

    //KEEP MUTATIONS
    setKeeps(state, keeps) {
      state.keeps = keeps;
    },

    addKeep(state, keep) {
      state.keeps.unshift(keep);
    },

    //VAULT MUTATIONS
    setVaults(state, vaults) {
      vaults.forEach(vault => {
        vault.keeps = []
        vault.dialog = false
      })
      state.vaults = vaults;
    },

    addVault(state, vault) {
      state.vaults.unshift(vault);
    },

    setActiveVaultId(state, vaultId) {
      state.activeVaultId = vaultId
    },


    //VAULT-KEEP MUTATIONS
    setVaultKeeps(state, vaultKeeps) {
      vaultKeeps.forEach(vk => {
        if (!state.vaultKeeps[vk.vaultId]) {
          Vue.set(state.vaultKeeps, vk.vaultId, [vk.keepId])
        }
        else {
          let tempArr = state.vaultKeeps[vk.vaultId]
          tempArr.push(vk.keepId)
          Vue.set(state.vaultKeeps, vk.vaultId, tempArr)
        }
      })
    },

    setActiveVaultKeeps(state, vaultId) {
      let tempArr = []
      if (!state.vaultKeeps[vaultId]) { state.activeVaultKeeps = [] }
      state.vaultKeeps[vaultId].forEach(keepId => {
        tempArr.push(state.keeps.find(keep => keep.id == keepId))
      })
      state.activeVaultKeeps = tempArr
    },

    updateActiveVaultKeeps(state, keep) {
      let index = state.activeVaultKeeps.findIndex(vkeep => vkeep.id == keep.id)
      if (index > -1) { state.activeVaultKeeps[index] = keep }
    },

    addVaultKeep(state, vk) {
      //update vaultkeeps dictionary
      if (!state.vaultKeeps[vk.vaultId]) {
        Vue.set(state.vaultKeeps, vk.vaultId, [vk.keepId])
      }
      else {
        let tempArr = state.vaultKeeps[vk.vaultId]
        tempArr.push(vk.keepId)
        Vue.set(state.vaultKeeps, vk.vaultId, tempArr)
      }
      //update activeVaultKeeps array as appropriate
      if (state.activeVaultId == vk.vaultId) {
        state.activeVaultKeeps.push(state.keeps.find(keep => keep.id == vk.keepId))
      }
    },

    removeVaultKeep(state, vk) {
      //update vaultkeeps dictionary
      let tempArr = state.vaultKeeps[vk.VaultId].filter(keepId => keepId != vk.KeepId)
      Vue.set(state.vaultKeeps, vk.VaultId, tempArr)

      //update activeVaultKeeps array
      if (state.activeVaultId == vk.VaultId) {
        state.activeVaultKeeps = state.activeVaultKeeps.filter(keep => keep.id != vk.KeepId)
      }
    }
  },

  actions: {

    //USER ACTIONS 

    register({ commit, dispatch }, newUser) {
      auth.post('Register', newUser)
        .then(res => {
          commit('setUser', res.data)
          router.push({ name: 'home' })
        })
        .catch(e => {
          console.log('[registration failed] :', e)
          alert(e.message)
        })
    },
    authenticate({ commit, dispatch }) {
      auth.get('authenticate')
        .then(res => {
          commit('setUser', res.data)
          router.push({ name: 'home' })
        })
        .catch(e => {
          console.log('not authenticated')
          //alert(e.message)
        })
    },
    login({ commit, dispatch }, creds) {
      auth.post('Login', creds)
        .then(res => {
          commit('setUser', res.data)
          router.push({ name: 'home' })
        })
        .catch(e => {
          console.log('Login Failed: ', e)
          alert(e.message)
        })
    },
    logout({ commit }) {
      auth.delete('Logout')
        .then(res => commit('logout'))
        .catch(err => console.error(err))
    },

    //KEEP ACTIONS
    getKeeps({ commit }) {
      api.get("Keep")
        .then(res => {
          commit("setKeeps", res.data)
        })
        .catch(err => (console.error(err)))
    },
    createKeep({ commit }, keep) {
      api.post("Keep", keep)
        .then(res => {
          commit("addKeep", res.data)
        })
        .catch(err => console.error(err))
    },

    deleteKeep({ dispatch }, keep) {
      api.delete("Keep", { data: keep })
        .then(res => {
          dispatch('getKeeps')
        })
        .catch(err => console.error(err))
    },

    updateKeep({ commit, dispatch }, keep) {
      api.put("Keep", keep)
        .then(res => {
          dispatch('getKeeps')
          commit('updateActiveVaultKeeps', keep)
        })
        .catch(err => console.log(err))
    },
    //VAULT ACTIONS
    getVaults({ commit }) {
      api.get('Vault')
        .then(res => {
          commit('setVaults', res.data)
        })
        .catch(err => console.error(err))
    },

    addVault({ commit }, vault) {
      api.post("vault", vault)
        .then(res => {
          commit("addVault", res.data)
        })
        .catch(err => console.error(err))
    },

    deleteVault({ dispatch }, vault) {
      api.delete("Vault", { data: vault })
        .then(res => {
          dispatch("getVaults")
        })
        .catch(err => console.log(err))

    },

    setActiveVault({ commit }, vaultId) {
      commit('setActiveVaultId', vaultId)
      commit('setActiveVaultKeeps', vaultId)
    },

    //VAULT-KEEP ACTIONS

    getVaultKeeps({ commit }) {
      api.get('VaultKeep')
        .then(res => {
          console.log("vaultkeeps ", res.data)
          commit("setVaultKeeps", res.data)
        })
        .catch(err => console.error(err))
    },

    addKeepToVault({ commit, dispatch, state }, vkUpdate) {
      let vaultKeep = vkUpdate.vaultAdd
      let keep = vkUpdate.keepUpdate
      //don't add a duplicate keep to a vault
      if (state.vaultKeeps[vaultKeep.VaultId] && state.vaultKeeps[vaultKeep.VaultId].includes(vaultKeep.KeepId)) {
        alert("This keep is already in the selected vault.")
      }
      else {
        keep.keeps++
        api.post("VaultKeep", vaultKeep)
          .then(res => {
            dispatch('updateKeep', keep)
            commit("addVaultKeep", res.data)
          })
          .catch(err => console.error(err))
      }
    },

    removeKeepFromVault({ commit }, vaultKeep) {
      api.delete("VaultKeep", { data: vaultKeep })
        .then(res => {
          commit("removeVaultKeep", vaultKeep)
        })
        .catch(err => console.error(err));
    }



  }
})