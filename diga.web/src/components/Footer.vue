<template>
  <footer>
    <b-container fluid class="py-4">
      <b-row class="mx-2">
        <div class="col-12 col-md-2">
          <img style="text-center" src="/Assets/platform/logo-footer.png" />
          <div class="d-flex my-5">
            <div class="mr-1">
              <a href="https://www.facebook.com/DIGA.PT" target="_blank"
                ><img src="/Assets/platform/icons/facebook.svg"
              /></a>
            </div>
            <div class="mx-1">
              <a :href="$t('whatsapp_number')" target="_blank"
                ><img src="/Assets/platform/icons/whatsapp.svg"
              /></a>
            </div>
            <div class="mx-1">
              <a :href="$t('youtube_channel')" target="_blank"
                ><img src="/Assets/platform/icons/youtube.svg"
              /></a>
            </div>
            <div class="ml-1">
              <a
                href="https://www.linkedin.com/company/diga-pt/about/"
                target="_blank"
                ><img src="/Assets/platform/icons/linkedin.svg"
              /></a>
            </div>
          </div>

          <b-row class="mt-5">
            <i class="col-2 fas fa-phone-alt"></i
            ><a
              class="col-10 orange font-weight-bold"
              :href="'tel:' + $t('phone_number')"
            >
              {{ $t("phone_number") }}
            </a>
          </b-row>
          <b-row class="mt-2">
            <i class="col-2 fas fa-envelope"></i
            ><a class="col-10" href="mailto:geral@diga.pt"
              ><span class="fs-18 font-weight-bold orange"
                >geral@diga.pt</span
              ></a
            >
          </b-row>
        </div>
        <div class="col-12 col-md-2">
          <p class="h3 mb-3">{{ $t("platform") }}</p>
          <ul class="list-unstyled">
            <li>
              <a href="#" class="t1">{{ $t("beginning_of_work") }}</a>
            </li>
            <li>
              <a href="#" class="t1">{{ $t("account_mgmt") }}</a>
            </li>
            <li>
              <a href="#" class="t1">{{ $t("for_customers") }}</a>
            </li>
            <li>
              <a href="#" class="t1">{{ $t("for_executors") }}</a>
            </li>
            <li>
              <a href="~/price/index" class="t1">{{
                $t("all_service_categories")
              }}</a>
            </li>
          </ul>
        </div>
        <div class="col-12 col-md-2">
          <p class="h3 mb-3">Полезное</p>
          <ul class="list-unstyled">
            <li>
              <a href="#" class="t1">Как заказать услуги</a>
            </li>
            <li>
              <a href="#" class="t1">Гарантии и безопасность</a>
            </li>
            <li>
              <a href="#" class="t1">Безопасные платежи</a>
            </li>
            <li>
              <a href="#" class="t1">Последние отзывы</a>
            </li>
            <li>
              <a href="#" class="t1">ТОП Исполнителей</a>
            </li>
            <li>
              <a href="#" class="t1">ТОП Заказчиков</a>
            </li>
          </ul>
        </div>
        <div class="col-12 col-md-2">
          <p class="h3 mb-3">Помощь</p>
          <ul class="list-unstyled">
            <li>
              <a href="#" class="t1">Вопросы и ответы</a>
            </li>
            <li>
              <a href="#" class="t1">Пользовательское соглашение</a>
            </li>
            <li>
              <a href="/home/privacy" class="t1">
                {{ $t("privacy_policy") }}
              </a>
            </li>
            <li>
              <a href="#" class="t1">Полезные ссылки</a>
            </li>
            <li>
              <a href="#" class="t1"> {{ $t("support") }} </a>
            </li>
          </ul>
        </div>
        <div class="col-12 col-md-3 text-center">
          <b-row class="mt-3">
            <a
              class="col-6"
              type="button"
              target="_blank"
              href="https://www.apple.com/app-store/"
              ><img src="/Assets/platform/img/apple.png"
            /></a>
            <a
              class="col-6 ml-auto"
              type="button"
              target="_blank"
              href="https://play.google.com"
              ><img src="/Assets/platform/img/googleplay.png"
            /></a>
          </b-row>
        </div>
        <b-col>
          <div class="d-flex justify-content-between">
            <div class="t4 align-self-end">
              &copy; {{ currentYear }} Copyright
              {{ $t("all_rights_reserved") }}
              <a href="#"> PEC.PT </a>
            </div>
            <div class=" t4">
              <a class="orange" href="/home/privacy" target="_blank">{{
                $t("privacy_policy")
              }}</a>
              <a
                class="orange d-block"
                href="/Assets/documents/ЛИЦЕНЗИОННЫЙ ДОГОВОР.pdf"
                >{{ $t("contract_offer") }}</a
              >
            </div>
          </div>
        </b-col>
      </b-row>
    </b-container>
  </footer>
</template>

<script>
export default {
  components: {},
  data() {
    return {
      currentYear: new Date().getFullYear(),
    };
  },
  methods: {},
};
</script>
<style>
/*FOOTER*/

footer {
  background-color: #2f2c33;
  color: #fff;
}

footer a {
  color: #fff;
}

footer a:hover,
footer a:active,
footer a:focus {
  color: #f16b24;
  font-weight: 700;
  text-decoration: none;
}

footer li {
  margin: 0.3rem 0;
}
</style>
