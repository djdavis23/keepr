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
  withCredentials: true
})

export default new Vuex.Store({
  state: {
    user: {},
    keeps: [],
    vaults: []
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
      state.vaults = vaults;
    },

    addVault(state, vault) {
      state.vaults.unshift(vault);
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

    }

  }
})