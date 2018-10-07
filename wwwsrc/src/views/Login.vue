<template>
    <v-layout row wrap class="header blue accent-2">
        <v-flex xs12 sm6 justify-start>
            <h1 class="mt-3 white--text font-weight-bold font-italic">UnPinterested</h1>
        </v-flex>
        <v-flex xs12 sm6 justify-end>
            <v-btn round @click="loginForm = !loginForm" class="btn btn-secondary mb-3 mt-3">Login</v-btn>
            <v-btn round @click="registerForm = !registerForm" class="btn btn-secondary ml-2 mb-3 mt-3">Register</v-btn>
        </v-flex>
        <v-flex xs12 sm6 offset-sm3>
            <v-card>
                <form v-if="loginForm" @submit.prevent="loginUser">
                    <input type="email" v-model="creds.email" placeholder="email">
                    <input type="password" v-model="creds.password" placeholder="password">
                    <v-btn round color="green accent-3" type="submit">Login</v-btn>
                </form>
            </v-card>
        </v-flex>
        <v-flex xs12 sm10 offset-sm1>
            <v-card>
                <form v-if="registerForm" @submit.prevent="register">
                    <input type="text" v-model="newUser.username" placeholder="name">
                    <input type="email" v-model="newUser.email" placeholder="email">
                    <input type="password" v-model="newUser.password" placeholder="password">
                    <v-btn round color="green accent-3" type="submit">Register</v-btn>
                </form>
            </v-card>
        </v-flex>
    </v-layout>
</template>

<script>
    export default {
        name: "login",
        mounted() {
            //checks for valid session
            this.$store.dispatch("authenticate");
        },
        data() {
            return {
                loginForm: false,
                registerForm: false,
                creds: {
                    email: "",
                    password: ""
                },
                newUser: {
                    email: "",
                    password: "",
                    username: ""
                }
            };
        },
        methods: {
            register() {
                this.$store.dispatch("register", this.newUser);
            },
            loginUser() {
                this.$store.dispatch("login", this.creds);
            }
        }

    };
</script>
<style>

</style>