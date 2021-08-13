<template>
  <div class="m-3" v-if="contract && contractorInfo && $store.state.user">
    <div class="orange-header px-4 py-3 d-flex">
      <span class="h1"> {{ $t("contract") }}: {{ contract.name }}</span>
      <div v-if="contract.contractTypeId" class="ml-auto d-flex contract-type">
        <span class="ctype fs-14">
          {{ $t(getContractTypeById(contract.contractTypeId).name) }}
        </span>
        <div class="trapezoid-left"></div>
      </div>
    </div>
    <template>
      <b-breadcrumb class="my-0 t3">
        <b-breadcrumb-item class=" blue" :to="{ name: 'home_index' }">
          <i class="bx bx-home"></i>
        </b-breadcrumb-item>

        <b-breadcrumb-item class="blue">{{ $t("Draft") }}</b-breadcrumb-item>
        <b-breadcrumb-item class="blue" disabled>{{
          $t("publication")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item class="blue">{{
          $t("choosing_contractor")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item class="blue">{{
          $t("estimate_approval")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item class="blue">{{
          $t("approval_of_conditions")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("Signing") }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("execution") }}</b-breadcrumb-item>
      </b-breadcrumb>
    </template>
    <div>
      <div class="bg-white py-3 px-4 my-1">
        <p class="h2 m-0">{{ $t("edit_conditions") }}</p>
      </div>
      <b-row>
        <b-col cols="9">
          <div class="card">
            <b-row class="align-items-end p-4">
              <b-col>
                <b-form-group
                  :disabled="isEdit === false"
                  :label="$t('general_terms')"
                >
                  <b-form-input
                    v-model="payment.generalTerm"
                    type="number"
                    class="mb-2"
                  ></b-form-input>
                </b-form-group>
              </b-col>
              <b-col>
                <b-form-group :label="$t('start_of_construction')">
                  <b-form-datepicker
                    :disabled="isEdit === false"
                    :min="beforeToday"
                    :locale="$root.locale"
                    v-model="payment.buildStart"
                    class="mb-2"
                  ></b-form-datepicker>
                </b-form-group>
              </b-col>

              <b-col>
                <b-form-group :label="$t('completion_of_construction')">
                  <b-form-datepicker
                    :disabled="isEdit === false"
                    :min="beforeToday"
                    :locale="$root.locale"
                    v-model="payment.plannedBuildEnd"
                    class="mb-2"
                  ></b-form-datepicker>
                </b-form-group>
              </b-col>
              <b-col class="m-auto" v-if="isDateError === true" cols="1">
                <i
                  v-b-tooltip.hover
                  :title="$t('days_error_text')"
                  class="bx bxs-help-circle blue"
                  style="font-size: x-large"
                ></i>
              </b-col>
            </b-row>
            <b-row class="p-4">
              <div class="col-12">
                <b-form-group :label="$t('executor')">
                  <b-input
                    class="blue-disabled"
                    disabled
                    :value="executorCard.name"
                  />
                </b-form-group>
              </div>
              <div class="col-12">
                <b-form-group :label="$t('contract_name')">
                  <b-input
                    class="blue-disabled"
                    disabled
                    :value="contract.name"
                  />
                </b-form-group>
              </div>
              <div v-if="contract.estimateName" class="col-12">
                <b-form-group :label="$t('estimate')">
                  <b-input
                    class="blue-disabled"
                    disabled
                    :value="contract.estimateName"
                  />
                </b-form-group>
              </div>

              <div class="col-12 mt-4">
                <b-form-group :label="$t('description')">
                  <div
                    v-if="isEdit === false"
                    class="my-2 blue-disabled"
                    v-html="contract.description"
                  ></div>
                  <vue-editor
                    v-else
                    @input="
                      errors.description = false;
                      errors.minlength = false;
                    "
                    :class="{
                      'input-invalid': errors.description || errors.minlength,
                    }"
                    v-model="contract.description"
                  >
                  </vue-editor>
                  <p
                    class="fs-13"
                    v-if="contract.description && isEdit === true"
                  >
                    {{ contract.description.length }} / {{ 400 }} ({{
                      $t("minlength")
                    }}400)
                  </p>
                  <p class="error text-red" v-if="errors.minlength">
                    {{ $t("minlength") }}400
                  </p>
                </b-form-group>
              </div>
            </b-row>
            <div class="mt-3">
              <div class="my-2">
                <b-form-group label-class="p-4" :label="$t('payment_stages')">
                  <b-button-group
                    class="d-flex justify-content-between py-2 px-4 bg_grey"
                  >
                    <b-button
                      :class="{
                        'b-blue-option-active': payment.paymentType === 'Safe',
                      }"
                      class="b-option"
                      @click="payment.paymentType = 'Safe'"
                      :disabled="isEdit === false"
                    >
                      {{ $t("Safe") }}
                    </b-button>
                    <b-button
                      :class="{
                        'b-orange-option-active':
                          payment.paymentType === 'Direct',
                      }"
                      class="b-option ml-1"
                      @click="payment.paymentType = 'Direct'"
                      :disabled="isEdit === false"
                    >
                      {{ $t("direct_payment") }}
                    </b-button>
                  </b-button-group>
                </b-form-group>
              </div>

              <div class="p-4" v-if="payment.paymentType === 'Safe'">
                <b-form-group
                  label-class="h5"
                  label-cols="1"
                  :label="$t('amount')"
                  content-cols="2"
                >
                  <b-input
                    class="blue-disabled"
                    disabled
                    :value="parseFloat(contract.price).toFixed(2) + ' EUR'"
                  />
                </b-form-group>
                <div class="my-3">
                  <b> {{ $t("payment_will_be_made_upon") }} </b>
                  <template v-if="$store.state.user.id === progress.executorId">
                    <p class="my-2">{{ $t("executor_safe_text") }}</p>
                    <p class="my-2">{{ $t("executor_safe_text2") }}</p>
                  </template>
                  <template v-else>
                    <p class="my-2">{{ $t("customer_safe_text") }}</p>
                    <p class="my-2">{{ $t("customer_safe_text_2") }}</p>
                  </template>
                </div>

                <b-form-group :label="$t('payment_of_fee')">
                  <b-form-radio-group
                    :disabled="isEdit === false"
                    stacked
                    v-model="payment.comissionType"
                    :options="feeOptions"
                  >
                  </b-form-radio-group>
                </b-form-group>

                <b-row class="my-2" align-v="center">
                  <b-col cols="3">
                    <p>
                      {{ $t("amount_with_safe_is") }}
                      <i
                        class="bx bxs-help-circle blue align-middle mx-1"
                        v-b-tooltip.hover
                        :title="$t('safe_fee_percent')"
                      ></i>
                    </p>
                  </b-col>
                  <b-col cols="2">
                    <b-input
                      class="blue-disabled"
                      disabled
                      :value="safe9Percent.toFixed(2) + ' EUR'"
                    />
                  </b-col>
                </b-row>

                <p>
                  <b> {{ $t("automatic_calculation") }} </b>
                  {{ $t("automatic_calculation_text") }}
                </p>
                <b-form-group
                  class="my-3"
                  :label="$t('select_payments_per_month')"
                >
                  <b-form-radio-group
                    stacked
                    :disabled="isEdit === false"
                    v-model="payment.paymentFrequency"
                    :options="mFrequencyOptions"
                  >
                  </b-form-radio-group>
                </b-form-group>
              </div>
              <!-- qqqqqqqqqqqqqqq -->
              <div class="p-4" v-if="payment.paymentType === 'Direct'">
                <b-form-group
                  label-class="p-4"
                  :label="$t('choose_cooperation_option')"
                >
                  <b-button-group class="d-flex justify-content-between px-4">
                    <b-button
                      :class="{
                        'b-blue-option-active':
                          payment.cooperationType === 'IndependentMeasurements',
                      }"
                      class="b-option"
                      @click="
                        payment.cooperationType = 'IndependentMeasurements'
                      "
                      :disabled="isEdit === false"
                    >
                      {{ $t("automatic_calculation") }}
                    </b-button>
                    <b-button
                      :class="{
                        'b-blue-option-active':
                          payment.cooperationType === 'ForStagesOfWork',
                      }"
                      class="b-option mx-1"
                      @click="payment.cooperationType = 'ForStagesOfWork'"
                      :disabled="isEdit === false"
                    >
                      {{ $t("for_milestones") }}
                    </b-button>
                    <b-button
                      :class="{
                        'b-blue-option-active':
                          payment.cooperationType === 'ForHours',
                      }"
                      class="b-option"
                      @click="payment.cooperationType = 'ForHours'"
                      :disabled="isEdit === false"
                    >
                      {{ $t("hourly_payment") }}
                    </b-button>
                  </b-button-group>
                  <hr />
                </b-form-group>

                <div
                  v-if="payment.cooperationType === 'IndependentMeasurements'"
                >
                  <p>
                    <b> {{ $t("automatic_calculation") }} </b>
                    {{ $t("automatic_calculation_text") }}
                  </p>
                  <b-form-group
                    label-cols="1"
                    :label="$t('amount')"
                    content-cols="2"
                  >
                    <b-input
                      class="blue-disabled"
                      disabled
                      :value="parseFloat(contract.price).toFixed(2) + ' EUR'"
                    />
                  </b-form-group>

                  <b-row class="my-3 align-items-center">
                    <b-col cols="3">
                      <span class="text-uppercase">
                        {{ $t("prepayment") }}
                        <i
                          class="bx bxs-help-circle blue align-middle mx-1"
                          v-b-tooltip.hover
                          :title="$t('amount_transferred_executor')"
                        ></i>
                      </span>
                    </b-col>
                    <b-col cols="3">
                      <b-form-group :disabled="isEdit === false">
                        <b-form-checkbox-group v-model="payment.isPrepayment">
                          <b-form-checkbox :value="true">
                            {{ $t("yes") }}
                          </b-form-checkbox>
                          <b-form-checkbox :value="false"
                            >{{ $t("no") }}
                          </b-form-checkbox>
                        </b-form-checkbox-group>
                      </b-form-group>
                    </b-col>
                    <b-col cols="2">
                      <b-form-input
                        :class="{
                          'input-invalid': $v.payment.prepaymentPercent.$error,
                        }"
                        :disabled="isEdit === false"
                        v-if="payment.isPrepayment === true"
                        v-model="payment.prepaymentPercent"
                        type="number"
                        step="0.1"
                        min="0"
                        max="100"
                        placeholder="10%"
                      />
                    </b-col>
                    <b-col cols="2">
                      <b-form-input
                        :class="{
                          'input-invalid': $v.payment.prepaymentValue.$error,
                        }"
                        :disabled="isEdit === false"
                        v-if="payment.isPrepayment === true"
                        v-model="payment.prepaymentValue"
                        type="number"
                        step="0.1"
                        min="0"
                        :placeholder="$t('amount')"
                      />
                    </b-col>
                  </b-row>

                  <b-form-group
                    class="my-3"
                    :label="$t('select_payments_per_month')"
                  >
                    <b-form-radio-group
                      stacked
                      :disabled="isEdit === false"
                      v-model="payment.paymentFrequency"
                      :options="mFrequencyOptions"
                    >
                    </b-form-radio-group>
                  </b-form-group>
                </div>
                <div
                  class="my-5"
                  v-if="payment.cooperationType === 'ForStagesOfWork'"
                >
                  <b-form-group
                    label-class="h5"
                    label-cols="2"
                    :label="$t('amount')"
                    content-cols="2"
                  >
                    <b-input
                      class="blue-disabled"
                      disabled
                      :value="parseFloat(contract.price).toFixed(2) + ' EUR'"
                    />
                  </b-form-group>

                  <b-row class="my-3" align-v="center">
                    <b-col cols="3">
                      <p class="h5">
                        {{ $t("prepayment") }}
                        <i
                          class="bx bxs-help-circle blue align-middle mx-1"
                          v-b-tooltip.hover
                          :title="$t('amount_transferred_executor')"
                        ></i>
                      </p>
                    </b-col>
                    <b-col cols="2">
                      <b-form-radio-group
                        :disabled="isEdit === false"
                        v-model="payment.isPrepayment"
                      >
                        <b-form-radio :value="true">
                          {{ $t("yes") }}
                        </b-form-radio>
                        <b-form-radio :value="false"
                          >{{ $t("no") }}
                        </b-form-radio>
                      </b-form-radio-group>
                    </b-col>
                    <b-col cols="2">
                      <b-form-input
                        :disabled="isEdit === false"
                        :class="{
                          'input-invalid': $v.payment.prepaymentPercent.$error,
                        }"
                        v-if="payment.isPrepayment === true"
                        v-model="payment.prepaymentPercent"
                        type="number"
                        step="0.1"
                        min="0"
                        placeholder="10%"
                      />
                    </b-col>
                    <b-col cols="2">
                      <b-form-input
                        :disabled="isEdit === false"
                        :class="{
                          'input-invalid': $v.payment.prepaymentValue.$error,
                        }"
                        v-if="payment.isPrepayment === true"
                        required
                        v-model="payment.prepaymentValue"
                        type="number"
                        step="0.1"
                        min="0"
                        :placeholder="$t('amount')"
                      />
                    </b-col>
                  </b-row>
                  <b-row>
                    <b-col cols="3">
                      <label> {{ $t("milestone_name") }} </label>
                    </b-col>
                    <b-col cols="2" class="px-1">
                      <label> {{ $t("amount") }} </label>
                    </b-col>
                    <b-col cols="1" class="px-1">
                      <label> % {{ $t("of_amount") }} </label>
                    </b-col>
                    <b-col cols="2" class="px-1">
                      <label> % {{ $t("of_completed_work") }} </label>
                    </b-col>

                    <b-col cols="4" class="px-1">
                      <label> {{ $t("date") }} </label>
                    </b-col>
                  </b-row>

                  <div
                    v-for="(milestone, index) in $v.milestones.$each.$iter"
                    :key="index"
                  >
                    <b-form-group :disabled="isEdit === false">
                      <b-row>
                        <b-col cols="3" class="px-1">
                          <b-form-input
                            :class="{
                              'input-invalid': milestone.name.$error,
                            }"
                            type="text"
                            v-model="milestone.name.$model"
                          >
                          </b-form-input
                        ></b-col>
                        <b-col cols="2" class="px-1">
                          <b-form-input
                            :class="{
                              'input-invalid': milestone.value.$error,
                            }"
                            type="number"
                            min="0"
                            step="0.1"
                            v-model="milestone.value.$model"
                            :value="contract.price * (milestone.percent / 100)"
                          >
                          </b-form-input>
                        </b-col>

                        <b-col cols="1" class="px-1">
                          <b-form-input
                            :class="{
                              'input-invalid': milestone.percent.$error,
                            }"
                            type="number"
                            min="0"
                            step="0.1"
                            max="100"
                            v-model="milestone.percent.$model"
                          >
                          </b-form-input
                        ></b-col>
                        <b-col cols="2" class="px-1">
                          <b-form-input
                            :class="{
                              'input-invalid': milestone.percentOfWork.$error,
                            }"
                            type="number"
                            min="0"
                            step="0.1"
                            max="100"
                            v-model="milestone.percentOfWork.$model"
                          >
                          </b-form-input
                        ></b-col>

                        <b-col cols="2" class="px-1">
                          <b-form-datepicker
                            :class="{
                              'input-invalid': milestone.dateTime.$error,
                            }"
                            :disabled="isEdit === false"
                            :date-format-options="{
                              year: 'numeric',
                              month: '2-digit',
                              day: '2-digit',
                            }"
                            :min="beforeToday"
                            v-model="milestone.dateTime.$model"
                          >
                          </b-form-datepicker
                        ></b-col>
                        <b-col
                          v-if="isEdit === true"
                          cols="2"
                          class="text-center px-0"
                        >
                          <b-button
                            class="mx-1 bg-white"
                            @click="confirmMilestonesLine(index)"
                            ><img src="/Assets/platform/icons/add.svg" alt="" />
                          </b-button>

                          <b-button
                            class="bg-white"
                            @click="deleteMilestonesLine(index)"
                          >
                            <img
                              src="/Assets/platform/icons/clear.svg"
                              alt=""
                            />
                          </b-button>
                        </b-col>
                      </b-row>
                    </b-form-group>
                  </div>
                  <b-form-group v-if="isEdit === true" class="col-1">
                    <b-button
                      type="button"
                      class="bg-white"
                      @click="addMilestonesLine"
                    >
                      <img src="/Assets/platform/icons/add.svg" alt="" />
                    </b-button>
                  </b-form-group>
                </div>
                <div v-if="payment.cooperationType === 'ForHours'">
                  <p>
                    <b> {{ $t("hourly_payment") }} </b>
                    {{ $t("hourly_payment_text") }}
                  </p>

                  <b-form-group
                    :disabled="isEdit === false"
                    label-cols="4"
                    content-cols="2"
                    label-class="fs-14 font-weight-bold align-middle"
                    :label="$t('specify_per_hour')"
                  >
                    <b-input-group>
                      <b-form-input
                        :class="{
                          'input-invalid': $v.payment.costOfHour.$error,
                        }"
                        v-model="payment.costOfHour"
                        class="border-right-0"
                        type="number"
                        :placeholder="$t('amount')"
                      ></b-form-input>
                      <template #append>
                        <b-input-group-text
                          class="bg-white border-left-0 border-radius-0"
                          >EUR</b-input-group-text
                        >
                      </template>
                    </b-input-group>
                  </b-form-group>
                  <b-form-group
                    label-class="fs-14 font-weight-bold align-middle"
                    class="my-3"
                    :label="$t('select_payments_per_month')"
                  >
                    <b-form-radio-group
                      stacked
                      :disabled="isEdit === false"
                      v-model="payment.paymentFrequency"
                      :options="hFrequencyOptions"
                    >
                    </b-form-radio-group>
                  </b-form-group>

                  <b-form-group
                    :disabled="isEdit === false"
                    label-cols="3"
                    content-cols="2"
                    label-class="fs-14 font-weight-bold align-middle"
                    :label="$t('planned_hours')"
                  >
                    <b-form-input
                      :placeholder="$t('enter_something')"
                      :class="{
                        'input-invalid': $v.payment.hoursOfWorks.$error,
                      }"
                      v-model="payment.hoursOfWorks"
                      type="number"
                    ></b-form-input>
                  </b-form-group>
                  <b-form-group
                    :disabled="isEdit === false"
                    label-cols="3"
                    content-cols="2"
                    label-class="fs-14 font-weight-bold align-middle"
                    :label="$t('estimated_budget')"
                  >
                    <b-input-group>
                      <b-form-input
                        class="border-right-0"
                        :placeholder="$t('amount')"
                        :class="{
                          'input-invalid': $v.payment.budgetOfWorks.$error,
                        }"
                        v-model="payment.budgetOfWorks"
                        type="number"
                      ></b-form-input>
                      <template #append>
                        <b-input-group-text
                          class="bg-white border-left-0 border-radius-0"
                          >EUR</b-input-group-text
                        >
                      </template>
                    </b-input-group>
                  </b-form-group>
                </div>
              </div>
            </div>
            <div class="d-flex p-4 justify-content-around">
              <b-button
                class="b-orange-outline"
                v-if="isEdit === false"
                :disabled="progress.contractCustomer === true"
                @click="isEdit = !isEdit"
                >{{ $t("edit") }}</b-button
              >
              <b-button
                v-if="isEdit === true"
                :disabled="progress.contractCustomer === true && $v.$invalid"
                class="b-blue-outline"
                @click="sendContractConfirmationChanges"
                >{{ $t("save_conditions") }}</b-button
              >

              <b-button
                class="b-blue mx-2"
                @click="saveContractChanges"
                :disabled="
                  ($store.state.user.id === progress.executorId &&
                    progress.contractCustomer === false) ||
                    (progress.contractCustomer === true && $v.$invalid)
                "
                >{{ $t("confirm_conditions") }}</b-button
              >
            </div>
          </div>
          <div class="card p-4">
            <div>
              <b-form-group label-class="p-4" :label="$t('contract')">
                <b-button-group
                  class="d-flex justify-content-between py-2 px-4 bg_grey"
                >
                  <b-button
                    :class="{
                      'b-blue-option-active': selected === 'chat',
                    }"
                    class="b-option"
                    @click="selected = 'chat'"
                  >
                    {{ $t("chat") }}
                  </b-button>
                  <b-button
                    :class="{
                      'b-orange-option-active': selected === 'history',
                    }"
                    class="b-option ml-1"
                    @click="selected = 'history'"
                  >
                    {{ $t("changes_history") }}
                  </b-button>
                </b-button-group>
              </b-form-group>
            </div>
            <div v-if="selected === 'history'">
              <history :id="id" />
            </div>
            <div v-else>
              <chat />
            </div>
          </div>
        </b-col>
        <b-col cols="3">
          <div class="card border-blue bg_grey">
            <div class="card-body p-0">
              <div v-if="author" class=" d-flex align-items-center m-3">
                <div>
                  <i
                    style="font-size: 60px"
                    width="60"
                    v-if="!author.avatar"
                    class=" bx bx-user-circle nav-i"
                  ></i>
                  <div
                    v-else
                    class="avatar "
                    v-bind:style="{
                      backgroundImage:
                        'url(' + '/img/src/' + author.avatar + ')',
                    }"
                  ></div>
                </div>
                <div class="mx-4">
                  <p class="h3">{{ $t("customer") }}</p>
                  <router-link class="blue" :to="{ name: 'profile' }">{{
                    author.fullName
                  }}</router-link>
                </div>
              </div>
              <div
                class="card-footer bg-white border-0 d-flex justify-content-around pb-4"
              >
                <span v-b-tooltip.hover :title="$t('Rating')" class="orange"
                  ><i class="bx bxs-star align-middle"></i
                  >{{ author.rating }}</span
                >

                <a
                  v-b-tooltip.hover
                  :title="$t('positive_reviews')"
                  class="mx-1"
                  href="#"
                  ><span class="blue"
                    ><i class="bx bxs-like  align-middle"></i>
                    {{ author.goodReviews }}</span
                  >
                </a>
                <a v-b-tooltip.hover :title="$t('negative_reviews')" href="#"
                  ><span class="grey-dark"
                    ><i class="bx bxs-dislike align-middle"></i>
                    {{ author.badReviews }}</span
                  ></a
                >
              </div>
              <div class="px-4 pt-0 pb-4 bg-white t1">
                <div
                  v-if="progress.contractCustomer === true"
                  class="confirmed p-2 text-center"
                >
                  <i class="bx bx-check-circle align-middle"></i>
                  {{ $t("conditions_confirmed") }}
                </div>
                <div v-else class="not-confirmed p-2 text-center">
                  <i class="bx bxs-minus-circle align-middle"></i>
                  {{ $t("under_consideration") }}
                </div>
              </div>
            </div>
          </div>
          <div class="card border-orange bg_grey">
            <div class="d-flex align-items-center m-3">
              <div>
                <i
                  style="font-size: 60px"
                  width="60"
                  v-if="!executorCard.avatar"
                  class=" bx bx-user-circle nav-i"
                ></i>
                <div
                  v-else
                  class="avatar"
                  v-bind:style="{
                    backgroundImage:
                      'url(' + '/img/src/' + executorCard.avatar + ')',
                  }"
                ></div>
              </div>
              <div class="mx-4">
                <p class="h3">{{ $t("executor") }}</p>
                <router-link class="orange" :to="{ name: 'profile' }">{{
                  executorCard.fullName
                }}</router-link>
              </div>
            </div>
            <div
              class="card-footer bg-white border-0 d-flex justify-content-around pb-4"
            >
              <span v-b-tooltip.hover :title="$t('Rating')" class="orange"
                ><i class="bx bxs-star align-middle"></i
                >{{ executorCard.rating }}</span
              >

              <a
                v-b-tooltip.hover
                :title="$t('positive_reviews')"
                class="mx-1"
                href="#"
                ><span class="blue"
                  ><i class="bx bxs-like  align-middle"></i>
                  {{ executorCard.goodReviews }}</span
                >
              </a>
              <a v-b-tooltip.hover :title="$t('negative_reviews')" href="#"
                ><span class="grey-dark"
                  ><i class="bx bxs-dislike align-middle"></i>
                  {{ executorCard.badReviews }}</span
                ></a
              >
            </div>
            <div class="px-4 pt-0 pb-4 bg-white t1">
              <div
                v-if="progress.contractExecutor === true"
                class="confirmed p-2 text-center"
              >
                <i class="bx bx-check-circle align-middle"></i>
                {{ $t("conditions_confirmed") }}
              </div>
              <div v-else class="not-confirmed p-2 text-center">
                <i class="bx bxs-minus-circle align-middle"></i>
                {{ $t("under_consideration") }}
              </div>
            </div>
          </div>

          <div class="card p-4">
            <p class="h3">{{ $t("contract_management") }}</p>

            <b-button
              v-if="isEdit === false && $store.state.user.id === progress.executorId"
              :disabled="progress.contractCustomer !== true"
              class="b-blue-outline my-2"
              @click="isEdit = !isEdit"
              >{{ $t("edit") }}</b-button
            >
             <b-button
              v-if="isEdit === false"
              :disabled="progress.contractCustomer === true"
              class="b-blue-outline my-2"
              @click="isEdit = !isEdit"
              >{{ $t("edit") }}</b-button
            >
            <b-button
              v-if="isEdit === true"
              :disabled="progress.contractCustomer === true && $v.$invalid"
              class="b-blue-outline my-2"
              @click="sendContractConfirmationChanges"
              >{{ $t("save_conditions") }}</b-button
            >

            <b-button
              class="b-blue my-2"
              @click="saveContractChanges"
              :disabled="
                ($store.state.user.id === progress.executorId &&
                  progress.contractCustomer === false) ||
                  (progress.contractCustomer === true && $v.$invalid)
              "
              >{{ $t("confirm_conditions") }}</b-button
            >

            <b-button
              v-if="$store.state.user.id === contractorInfo.userId"
              @click="refuse()"
              class="b-orange my-2"
              >{{ $t("refuse_cooperate") }}</b-button
            >
            <b-button v-else @click="refuseQuick()" class="b-orange my-2">{{
              $t("refuse_cooperate")
            }}</b-button>
            <refusal :id="id" :refuseModalShow="refuseModalShow" />
          </div>
        </b-col>
      </b-row>
    </div>
  </div>
</template>

<script>
import UserService from "@/services/UserService.js";
import { VueEditor } from "vue2-editor";
import { mapGetters, mapState } from "vuex";
import ContractService from "@/services/ContractService.js";
import refusal from "./Refusal.vue";
import history from "./History.vue";
import chat from "../Chat.vue";
import { requiredIf } from "vuelidate/lib/validators";

export default {
  props: ["id"],
  components: {
    VueEditor,
    refusal,
    history,
    chat,
  },
  validations: {
    payment: {
      prepaymentPercent: {
        required: requiredIf(function() {
          if (this.payment.isPrepayment === true) return true;
        }),
      },
      prepaymentValue: {
        required: requiredIf(function() {
          if (this.payment.isPrepayment === true) return true;
        }),
      },
      costOfHour: {
        required: requiredIf(function() {
          if (this.payment.cooperationType === "ForHours") return true;
        }),
      },
      hoursOfWorks: {
        required: requiredIf(function() {
          if (this.payment.cooperationType === "ForHours") return true;
        }),
      },
      budgetOfWorks: {
        required: requiredIf(function() {
          if (this.payment.cooperationType === "ForHours") return true;
        }),
      },
    },
    milestones: {
      $each: {
        name: {
          required: requiredIf(function() {
            if (this.payment.cooperationType === "ForStagesOfWork") return true;
          }),
        },
        value: {
          required: requiredIf(function() {
            if (this.payment.cooperationType === "ForStagesOfWork") return true;
          }),
        },
        percent: {
          required: requiredIf(function() {
            if (this.payment.cooperationType === "ForStagesOfWork") return true;
          }),
        },
        percentOfWork: {
          required: requiredIf(function() {
            if (this.payment.cooperationType === "ForStagesOfWork") return true;
          }),
        },
        dateTime: {
          required: requiredIf(function() {
            if (this.payment.cooperationType === "ForStagesOfWork") return true;
          }),
        },
      },
    },
  },
  data() {
    return {
      isEdit: false,
      payment: null,
      milestones: [],
      selected: "history",
      chatAndHistory: [
        { text: this.$t("chat"), value: "chat" },
        { text: this.$t("changes_history"), value: "history" },
      ],
      feeOptions: [
        {
          text: this.$t("i_pay_fee"),
          value: "PaidByCustomer",
        },
        {
          text: this.$t("fee_in_half"),
          value: "InHalf",
        },
        {
          text: this.$t("fee_executor"),
          value: "PaidByExecutor",
        },
      ],
      mFrequencyOptions: [
        {
          text: this.$t("weekly"),
          value: "Weekly",
        },
        {
          text: this.$t("biweekly"),
          value: "OnceInTwoWeeks",
        },
        {
          text: this.$t("monthly"),
          value: "Monthly",
        },
      ],
      hFrequencyOptions: [
        {
          text: this.$t("weekly"),
          value: "Weekly",
        },
        {
          text: this.$t("biweekly"),
          value: "OnceInTwoWeeks",
        },
        {
          text: this.$t("monthly"),
          value: "Monthly",
        },
        {
          text: this.$t("daily"),
          value: "Daily",
        },
      ],

      beforeToday: new Date().toISOString().substr(0, 10),
      contract: {},
      progress: {},
      executorCard: {},
      author: {},
      contractorInfo: null,
      errors: {},
      refuseModalShow: false,
    };
  },
  methods: {
    workPeriod() {
      var a = this.$moment(this.payment.buildStart);
      var b = this.$moment(this.payment.plannedBuildEnd);

      return b.diff(a, "days");
    },
    refuseQuick() {
      if (confirm(this.$t("are_you_sure_refuse"))) {
        ContractService.refuse(this.id)
          .then(() => {
            this.$toasted.success(
              this.$t("your_request_was_successfully_sent")
            );
            this.$router.push({ name: "allContracts" });
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
    refuse() {
      if (confirm(this.$t("are_you_sure_refuse"))) {
        this.refuseModalShow = true;
      }
    },
    confirmRefuse() {
      ContractService.refuse(this.id, { refusalCase: this.refusalCase })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.$router.push({ name: "allContracts" });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },

    sendContractConfirmationChanges() {
      this.$v.$touch();
      if (this.$v.$invalid) {
        this.$toasted.error(this.$t("oops_error"));
      } else {
        this.editContractChanges();
        this.postPaymentParts();
        this.isEdit = !this.isEdit;
      }
    },
    editContractChanges() {
      ContractService.editContractChanges(this.id, this.payment)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.getProgress();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    postPaymentParts() {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.postPaymentParts(this.id, {
        isPrepayment: this.payment.isPrepayment,
        prepaymentPercent: this.payment.prepaymentPercent,
        prepaymentValue: this.payment.prepaymentValue,
        parts: this.milestones,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getContractChanges() {
      ContractService.getContractChanges(this.id)
        .then((response) => {
          this.payment = response.data;
          if (this.payment.generalTerm === null) {
            this.payment.generalTerm = this.workPeriod();
          }
          if (this.payment.paymentFrequency === null) {
            this.payment.paymentFrequency = "Weekly";
          }
          this.milestones = response.data.paymentParts;
          this.milestones.forEach((m) => {
            m.percent = parseFloat(
              (m.value / response.data.price) * 100
            ).toFixed(2);
            if (m.dateTime < this.beforeToday) {
              m.dateTime = this.beforeToday;
            }
          });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    saveContractChanges() {
      ContractService.saveContractChanges(this.id)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.getProgress();
          this.getContract();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    confirmMilestonesLine(milestone) {
      if (
        (milestone.percent == null || milestone.percent === 0) &&
        milestone.value > 0
      ) {
        milestone.percent = parseFloat(
          (milestone.value / this.contract.price) * 100
        ).toFixed(2);
      }
      if (
        (milestone.value == null || milestone.value === 0) &&
        milestone.percent > 0
      ) {
        milestone.value = parseFloat(
          (milestone.percent / 100) * this.contract.price
        ).toFixed(2);
      }
    },
    deleteMilestonesLine(index) {
      if (index > -1) {
        this.milestones.splice(index, 1);
      }
    },
    addMilestonesLine() {
      this.milestones.push({
        name: "",
        value: null,
        percent: null,
        dateTime: new Date(),
      });
    },
    validateForm() {
      if (this.contract.description) {
        if (this.contract.description.length >= 400) {
          return true;
        }
      }
      this.errors = {};
      if (!this.contract.description) {
        this.errors.description = true;
      } else if (this.contract.description.length < 400) {
        this.errors.minlength = true;
      }
      return false;
    },
    getContractor() {
      ContractService.getContractor(this.id)
        .then((response) => {
          this.contractorInfo = response.data;
          this.getCard();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getCard() {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.getCard(this.contractorInfo.userId)
        .then((response) => {
          this.author = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getExecutorCard() {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.getCard(this.progress.executorId)
        .then((response) => {
          this.executorCard = response.data;
          if (
            this.contractorInfo.userId !== this.$store.state.user.id &&
            this.progress.executorId !== this.$store.state.user.id
          ) {
            this.$toasted.error(this.$t("access_denied"));
            this.$router.push({ name: "allContracts" });
          }
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getProgress() {
      ContractService.getProgress(this.id)
        .then((response) => {
          this.progress = response.data;
          this.getExecutorCard();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    checkContractStatus() {
      switch (this.contract.status) {
        case "Draft":
          this.$router.push({
            name: "Draft",
            params: { id: this.id },
          });
          break;
        case "Deferred":
        case "Archived":
          this.$router.push({
            name: "allContracts",
          });
          break;
        case "Contractor":
          this.$router.push({
            name: "Contractor",
            params: { id: this.id },
          });
          break;
        case "EstimateApproval":
          this.$router.push({
            name: "EstimateApproval",
            params: { id: this.id },
          });
          break;
        // case "ContractApproval":
        //   this.$router.push({
        //     name: "confirmation",
        //     params: { id: this.id },
        //   });
        //   break;
        case "Signing":
          this.$router.push({
            name: "Signing",
            params: { id: this.id },
          });
          break;
        case "Executing":
          this.$router.push({
            name: "Executing",
            params: { id: this.id },
          });
          break;
        case "Refusal":
          this.$router.push({
            name: "ContractHistory",
            params: { id: this.id },
          });
          break;
        case "Closed":
          this.$router.push({
            name: "allContracts",
          });
          break;
        case "Finished":
          this.$router.push({
            name: "Finish",
            params: { id: this.id },
          });
          break;
      }
    },
    getContract() {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.getContract(this.id)
        .then((response) => {
          this.contract = response.data;
          this.checkContractStatus();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  watch: {
    "payment.prepaymentPercent": {
      handler() {
        if (this.payment.prepaymentPercent > 0) {
          if (this.payment.prepaymentPercent > 100) {
            this.payment.prepaymentPercent = 100;
          }

          this.payment.prepaymentValue = parseFloat(
            this.contract.price * (this.payment.prepaymentPercent / 100)
          ).toFixed(2);
        }
        if (this.payment.prepaymentPercent < 0) {
          this.payment.prepaymentPercent = 1;
        }
      },
    },
    "payment.buildStart": {
      handler() {
        let days = this.workPeriod();
        if (days > 0) {
          this.payment.generalTerm = days;
        }
      },
    },
    "payment.plannedBuildEnd": {
      handler() {
        let days = this.workPeriod();

        if (days > 0) {
          this.payment.generalTerm = days;
        }
      },
    },
  },
  computed: {
    isDateError() {
      let realDays = this.workPeriod();
      if (parseInt(realDays) !== parseInt(this.payment.generalTerm)) {
        return true;
      }
      return false;
    },
    ...mapGetters([
      "getCityById",
      "getCountryById",
      "getContractPriorityById",
      "getContractTypeById",
      "getBudgetById",
      "getVatById",
    ]),
    ...mapState(["vats"]),
    safe9Percent() {
      let result = 0;
      if (this.contract && this.contract.price) {
        if (this.payment.comissionType === "PaidByCustomer") {
          result = this.contract.price + (this.contract.price / 100) * 9;
        }
        if (this.payment.comissionType === "InHalf") {
          result = this.contract.price + (this.contract.price / 100) * 4.5;
        }
        if (this.payment.comissionType === "PaidByExecutor") {
          result = this.contract.price;
        }
        return result;
      }
      return result;
    },
    totalSumWithoutPrepayment() {
      if (this.contract && this.contract.price) {
        if (this.isPrepayment === true) {
          return this.contract.price - this.prepayment;
        } else {
          return this.contract.price;
        }
      } else {
        return 0;
      }
    },
    calculationOfValue() {
      let percent = this.milestones.percent / 100;
      let value = this.contract.price * percent;
      if (
        this.contract &&
        this.contract.price &&
        this.milestones &&
        this.milestones.percent &&
        this.milestones.percent.length > 0
      ) {
        return value;
      } else {
        return 0;
      }
    },
  },
  mounted() {
    this.getContract();
    this.getContractor();
    this.getProgress();
    this.getContractChanges();
  },
};
</script>
<style>
.btn-circle {
  width: 38px;
  border-radius: 25px;
}
.b-option {
  background-color: white !important;
  color: #2f2c33 !important;
  border: 1px solid #606161 !important;
}
.b-blue-option-active {
  background-color: #ccebec !important;
  color: #2f2c33 !important;
  border: 1px solid #438d90 !important;
}

.b-orange-option-active {
  background-color: #fdd6c1 !important;
  color: #2f2c33 !important;
  border: 1px solid #f16b24 !important;
}
</style>
